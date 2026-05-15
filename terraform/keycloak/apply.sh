#!/bin/sh
terraform init -input=false

terraform apply \
  -auto-approve \
  -input=false \
  -var-file=docker-compose.tfvars
