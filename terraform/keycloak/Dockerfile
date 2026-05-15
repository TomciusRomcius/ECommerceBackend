FROM hashicorp/terraform:1.9

RUN apk add --no-cache curl

COPY apply.sh /apply.sh
RUN chmod +x /apply.sh

WORKDIR /workspace

ENTRYPOINT ["/apply.sh"]
