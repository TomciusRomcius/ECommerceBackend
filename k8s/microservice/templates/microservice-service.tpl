apiVersion: v1
kind: Service
metadata:
  name: {{ .Values.app.name }}
  labels:
    app.kubernetes.io/managed-by: Helm
  annotations:
    meta.helm.sh/release-name: umbrella
    meta.helm.sh/release-namespace: default
spec:
  selector:
    app: {{ .Values.app.name }}
  ports:
    - protocol: TCP
      port: 8080
      targetPort: 8080
