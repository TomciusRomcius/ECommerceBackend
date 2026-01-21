# K8s Architecture (Not Production-Ready Yet)
## High-Level Overview
Kubernetes deployments are managed using Helm. To deploy the application, the umbrella chart must be installed.

Deployment Steps
```bash
cd k8s/umbrella
helm dependency update
helm install [deployment name]
```

> **Note:** ConfigMaps and Secrets must be created before deploying the umbrella chart.
The umbrella chart does not handle the creation of ConfigMaps and Secrets.

## Microservice Chart

To avoid duplicating Kubernetes configuration across C# microservices, a generic Helm chart located at /k8s/microservice/ is used. It sets up a deployment, service, and optionally a database migration job for each microservice.

Below is an example of configurable values available in a microservice’s values.yaml file:

```yaml
name: [name]
image: [image]

configMaps:
  - [configMap1]
  - [configMap2]
  ...

secrets:
  - [secret1]
  - [secret2]
  ...

migrationJob: # If enabled, a job defined in the Docker image will run on launch
  enabled: true
  name: [name]
  image: [image]
  backoffLimit: [backoffLimit]
  configMaps:
    - [configMap1]
    - [configMap2]
    ...
  secrets:
    - [secret1]
    - [secret2]
    ...
```
## Umbrella Chart

The umbrella chart:

1. Aggregates all microservice chart dependencies

2. Manages shared configuration via values.yaml

3. Deploys the full application stack