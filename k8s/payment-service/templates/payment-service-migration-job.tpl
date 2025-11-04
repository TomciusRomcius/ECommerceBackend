apiVersion: batch/v1
kind: Job
metadata:
  name: payment-service-db-migration-job
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
        - image: ecommercebackend-payment-service-db-migrator:prod
          name: payment-service-db-migrator
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