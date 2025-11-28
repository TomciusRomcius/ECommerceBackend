{{ if .Values.migrationJob.enabled }}
apiVersion: batch/v1
kind: Job
metadata:
  name: {{ .Values.migrationJob.name }}
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
        - image: {{ .Values.migrationJob.image }}
          name: {{ .Values.migrationJob.name }}
          imagePullPolicy: IfNotPresent
          args:
            - --
            - --host=$(Database__Host)
            - --database=$(Database__Database)
            - --username=$(Database__Username)
            - --password=$(Database__Password)
          envFrom:
            {{- range $i, $name := .Values.migrationJob.configMaps }}
            - configMapRef:
                name: {{ $name }}
            {{- end }}
            {{- range $i, $name := .Values.migrationJob.secrets }}
            - secretRef:
                name: {{ $name }}
            {{- end }}
{{ end }}
