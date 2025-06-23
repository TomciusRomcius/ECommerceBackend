apiVersion: v1
kind: Service
metadata:
  name: main-api-service
  labels:
    app.kubernetes.io/managed-by: Helm
  annotations:
    meta.helm.sh/release-name: umbrella
    meta.helm.sh/release-namespace: default
spec:
  type: NodePort
  selector:
    app: main-api
  ports:
    - port: 5042
      targetPort: 8080
