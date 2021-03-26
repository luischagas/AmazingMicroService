# Amazing MicroService

Para execução do projeto é necessário realizar o seguinte passo:

## Instalação

Criar um container do RabbitMQ:

```bash
docker run -d -p 15672:15672 -p 5672:5672 -p 5671:5671 --hostname my-rabbitmq --name my-rabbitmq-container -e RABBITMQ_DEFAULT_USER=USER -e RABBITMQ_DEFAULT_PASS=PASSWORD rabbitmq:3-management-alpine
```

## O que foi implementado?

- Arquiterura Clean
- RabbitMQ
- Testes
- Logger
- Docker

## Opcional

Caso queira executar duas instâncias do mesmo serviço hospedado no docker, bastar executar o seguinte comando, no diretório da aplicação:

```bash
docker-compose up
```
