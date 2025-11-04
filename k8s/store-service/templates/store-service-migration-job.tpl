apiVersion: batch/v1
kind: Job
metadata:
  name: store-service-db-migration-job
  labels:
    app.kubernetes.io/managed-by: Helm
  annotations:
    meta.helm.sh/release-name: umbrella
    meta.helm.sh/release-namespace: default
spec:
  backoffLimit: {{ default 5 .Values.migrationJob.backoffLimit }}
  template:
    spec:
      restartPolicy: OnFailure
      containers:
        - image: ecommercebackend-store-service-db-migrator:prod
          name: store-service-db-migrator
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