apiVersion: apps/v1
kind: Deployment
metadata:
  name: store-service-deployment
  labels:
    app: store-service
    app.kubernetes.io/managed-by: Helm
  annotations:
    meta.helm.sh/release-name: umbrella
    meta.helm.sh/release-namespace: default
spec:
  selector:
    matchLabels:
      app: store-service
  template:
    metadata:
      labels:
        app: store-service
    spec:
      containers:
        - image: ecommercebackend-store-service:prod
          name: store-service
          imagePullPolicy: IfNotPresent
          envFrom:
            - configMapRef:
                name: store-service-config
            - secretRef:
                name: stripe-secret
          env:
            - name: Database__Password
              valueFrom:
                secretKeyRef:
                  name: store-service-db-secret
                  key: Database__Password
            - name: Kafka__Servers
              value: {{ .Values.Kafka__Servers }}
          resources: # TODO: configure resources
