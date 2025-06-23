apiVersion: batch/v1
kind: Job
metadata:
  name: main-api-db-migration-job
  labels:
    app.kubernetes.io/managed-by: Helm
  annotations:
    meta.helm.sh/release-name: umbrella
    meta.helm.sh/release-namespace: default
spec:
  backoffLimit: 4
  template:
    spec:
      restartPolicy: OnFailure
      containers:
        - image: ecommercebackend-main-api-db-migrator:prod
          name: main-api-db-migrator
          imagePullPolicy: IfNotPresent
          envFrom:
            - configMapRef:
                name: main-api-config
            - secretRef:
                name: master-user-secrets
          env:
            - name: Database__Password
              valueFrom:
                secretKeyRef:
                  name: main-api-db-secret
                  key: Database__Password