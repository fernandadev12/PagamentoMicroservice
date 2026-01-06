# Microserviço de Pagamento  - FCG Fiap Cloud Games

Este é um microserviço responsável por gerenciar as operações relacionadas aos pagamentos de usuários na plataforma FCG Fiap Cloud Games. Ele é parte de uma arquitetura de microsserviços que inclui quatro microsserviços distintos, cada um residindo em seu próprio repositório Git.

## Funcionalidades

- **Autenticação**: O microserviço permite que os usuários se autenticem usando suas credenciais, como nome de usuário e senha. Ele retorna um token de acesso (JWT) para uso em solicitações subsequentes.

- **Cadastro de Pagamentos**: Os usuários podem vincular pagamentos no sistema, fornecendo informações como meio de pagamento. O microserviço valida as informações enviando email de confirmação para o usuário.

- **Efetuar pagamentos**: O microserviço é responsável por autorizar as solicitações dos pagamentos, efetuando  pagamentos.

## Tecnologias utilizadas

- **Linguagem de Programação**: C#  / .Net Core 8
- **Banco de Dados**: SqLite
- **Autenticação**: JSON Web Tokens (JWT)
- **Deploys**: Docker
- **Orquestração**: Kubernetes

## Como executar o projeto

1. Certifique-se de ter o Docker e o Kubernetes instalados em seu sistema.
2. Clone este repositório.
3. Execute `docker-compose up` para iniciar o microserviço.
4. O microserviço estará disponível em `http://localhost:5000`.

## Documentação da API

Para obter mais informações sobre a API do microserviço de usuários, consulte a documentação completa no arquivo `docs/api.md`.

## Contribuição

Contribuições são sempre bem-vindas! Se você encontrar algum problema ou tiver alguma sugestão de melhoria, por favor, abra uma issue ou faça um fork deste repositório.

## Licença

Este projeto está licenciado sob a [MIT License](LICENSE).
