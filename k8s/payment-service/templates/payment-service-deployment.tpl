apiVersion: apps/v1
kind: Deployment
metadata:
  name: payment-service-deployment
  labels:
    app: payment-service
    app.kubernetes.io/managed-by: Helm
  annotations:
    meta.helm.sh/release-name: umbrella
    meta.helm.sh/release-namespace: default
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
        - image: ecommercebackend-payment-service:prod
          name: payment-service
          imagePullPolicy: IfNotPresent
          envFrom:
            - configMapRef:
                name: payment-service-config
            - secretRef:
                name: stripe-secret
          env:
            - name: Database__Password
              valueFrom:
                secretKeyRef:
                  name: payment-service-db-secret
                  key: Database__Password
            - name: Kafka__Servers
              value: {{ .Values.Kafka__Servers }}
          resources: # TODO: configure resources
