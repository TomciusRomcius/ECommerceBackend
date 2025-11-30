apiVersion: networking.k8s.io/v1
kind: Ingress
metadata:
  name: cluster-ingress
  labels:
    app.kubernetes.io/managed-by: Helm
  annotations:
    meta.helm.sh/release-name: umbrella
    meta.helm.sh/release-namespace: default
spec:
  ingressClassName: nginx
  rules:
  - http:
      paths:
      - path: /user-service
        pathType: Prefix
        backend:
          service:
            name: user-service
            port:
              number: 8080
      - path: /product-service
        pathType: Prefix
        backend:
          service:
            name: product-service
            port:
              number: 8080
      - path: /store-service
        pathType: Prefix
        backend:
          service:
            name: store-service
            port:
              number: 8080
      - path: /order-service
        pathType: Prefix
        backend:
          service:
            name: order-service
            port:
              number: 8080
      - path: /payment-service
        pathType: Prefix
        backend:
          service:
            name: payment-service
            port:
              number: 8080
