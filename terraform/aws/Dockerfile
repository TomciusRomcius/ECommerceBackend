FROM alpine:3.23

RUN apk add --no-cache ca-certificates unzip wget

ARG TERRAFORM_VERSION
RUN wget -q "https://releases.hashicorp.com/terraform/1.9.8/terraform_1.9.8_linux_amd64.zip" \
  && unzip -q "terraform_1.9.8_linux_amd64.zip" \
  && mv terraform /usr/bin/terraform \
  && rm "terraform_1.9.8_linux_amd64.zip"

RUN apk add --no-cache python3 py3-pip \
  && ln -sf python3 /usr/bin/python \
  && pip install --break-system-packages terraform-local

WORKDIR /workspace
COPY . .
RUN chmod +x apply.sh

ENTRYPOINT ["/workspace/apply.sh"]
