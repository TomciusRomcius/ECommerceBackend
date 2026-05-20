terraform {
  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "6.22"
    }
  }
}

provider "aws" {
  region            = "us-east-1"
  s3_use_path_style = true
}
resource "aws_s3_bucket" "ecommerce_resources" {
  bucket = "ecommerce-resources"
}

resource "aws_s3_bucket_public_access_block" "ecommerce_resources" {
  bucket = aws_s3_bucket.ecommerce_resources.id
  block_public_acls = false
  skip_destroy = false
}