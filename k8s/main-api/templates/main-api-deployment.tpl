apiVersion: apps/v1
kind: Deployment
metadata:
  name: main-api-deployment
  labels:
    app: main-api
spec:
  selector:
    matchLabels:
      app: main-api
  template:
    metadata:
      name: main-api-pod
      labels:
        app: main-api
    spec:
      containers:
        - image: main-api:latest
          name: main-api
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
                  key: postgres-password
          resources:
            requests:
              cpu: "0.5"
              memory: "0.5Gi"
            limits:
              cpu: "1"
              memory: "1Gi"
