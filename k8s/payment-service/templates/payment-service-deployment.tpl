apiVersion: apps/v1
kind: Deployment
metadata:
  name: payment-service-deployment
  labels:
    app: payment-service
spec:
  selector:
    matchLabels:
      app: payment-service
  template:
    metadata:
      labels:
        app: payment-service
    spec:
      containers:
        - image: payment-service:latest
          name: payment-service
          imagePullPolicy: IfNotPresent
          env:
            - name: ASPNETCORE_URLS
              value: http://+:8080
            - name: ASPNETCORE_ENVIRONMENT
              value: {{ .Values.environment | quote }}
            - name: Stripe__ApiKey
              valueFrom:
                secretKeyRef:
                  name: app-secrets
                  key: Stripe__ApiKey
            - name: Stripe__WebhookSecret
              valueFrom:
                secretKeyRef:
                  name: app-secrets
                  key: Stripe__WebhookSecret
            - name: Kafka__Servers
              value: {{ .Values.Kafka__Servers }}
          resources: # TODO: configure resources
