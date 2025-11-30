apiVersion: apps/v1
kind: Deployment
metadata:
  name: {{ .Values.app.name }}
  labels:
    app: {{ .Values.app.name }}
    app.kubernetes.io/managed-by: Helm
    app.kubernetes.io/name: {{ .Values.app.name }}
  annotations:
    meta.helm.sh/release-name: umbrella
    meta.helm.sh/release-namespace: default
spec:
  selector:
    matchLabels:
      app: {{ .Values.app.name }}
  template:
    metadata:
      labels:
        app: {{ .Values.app.name }}
    spec:
      containers:
        - image : {{ .Values.app.image }}
          name: {{ .Values.app.name }}
          imagePullPolicy: IfNotPresent
          envFrom:
            {{- range $i, $name := .Values.app.configMaps}}
            - configMapRef:
                name: {{ $name }}
            {{- end }}
            {{- range $i, $name := .Values.app.secrets}}
            - secretRef:
                name: {{ $name }}
            {{- end }}
          resources: # TODO: configure resources
