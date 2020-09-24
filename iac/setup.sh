#!/bin/bash

# customize ACR_NAME to ensure globally unique name

RG_NAME="rg-basta-2020-keda"
AZ_REGION="westeurope"

ACR_NAME="basta2020keda"
AKS_NAME="basta2020keda"

az group create -n $RG_NAME -l $AZ_REGION

az acr create -n $ACR_NAME -g $RG_NAME -l $AZ_REGION --sku Basic --admin-enabled false

ACR_ID=$(az acr show -n $ACR_NAME -g $RG_NAME --query 'id' -o tsv)

az aks create -n $AKS_NAME -g $RG_NAME -l $AZ_REGION --enable-managed-identity --attach-acr $ACR_ID --node-count 2

az storage account create -n basta2020keda -g $RG_NAME -l $AZ_REGION --access-tier Hot --sku Standard_LRS