#!/usr/bin/env bash

cd deployment/rabbit-mq/
ls -a
docker-compose --env-file .env.dev stop

cd ../postgresql/
docker-compose --env-file .env.dev stop

cd ../seq/
docker-compose --env-file .env.dev stop

