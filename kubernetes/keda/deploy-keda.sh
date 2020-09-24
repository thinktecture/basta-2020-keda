#!/bin/bash

helm repo add kedacore https://kedacore.github.io/charts
helm repo update

# create a dedicated namespace for keda
kubectl create namespace keda

# install keda using helm3
helm install keda kedacore/keda --namespace keda