# API de Dados Epidemiológicos (ApiFiotec)

Este documento descreve como configurar, inicializar e utilizar a API para consulta de dados epidemiológicos relacionados a doenças como Dengue, Chikungunya e Zika.

## Configuração Inicial

### Restauração do Banco de Dados

1. Localize o arquivo `ApiFiotec.mdf` fornecido no projeto
2. Abra o SQL Server Management Studio (SSMS)
3. Clique com o botão direito em "Databases" e selecione "Restore Database..."
4. Na janela de restauração, selecione "Device" como fonte
5. Clique no botão "..." para navegar e selecionar o arquivo `ApiFiotec.mdf`
6. Confirme o nome do banco de dados de destino
7. Clique em "OK" para iniciar a restauração

### Executando as Migrations

Para garantir que o esquema do banco de dados esteja atualizado, execute as migrations do Entity Framework:

```bash
# Navegue até a pasta do projeto
cd caminho/para/ApiFiotec

# Execute o comando de migrations com log detalhado
dotnet ef database update -v
```

### Iniciando a API

Para executar a API .NET Core 8:

```bash
# Navegue até a pasta do projeto
cd caminho/para/ApiFiotec

# Opção 1: Executar em modo de desenvolvimento
dotnet run

# Opção 2: Executar com configuração específica
dotnet run --configuration Release

# Opção 3: Executar com watch (recompila automaticamente ao detectar alterações no código)
dotnet watch run
```

Por padrão, a API estará disponível em `http://localhost:5119`.

## Dados para Teste

Use os seguintes CPFs e nomes para autenticação nas requisições:

| CPF         | Nome     |
|-------------|----------|
| 22822770077 | Weaur    |
| 93478454000 | Gaicie   |
| 87975618004 | Payve    |
| 99657796008 | Waiss    |
| 99219006090 | Xayflumo |

## Endpoints Disponíveis

### 1. Lista de Estados

Retorna todos os estados disponíveis na base de dados.

```bash
curl -X 'GET' \
  'http://localhost:5119/api/v1/estados' \
  -H 'accept: */*'
```

### 2. Municípios por Estado

Retorna todos os municípios de um estado específico. No exemplo abaixo, consultamos os municípios do Rio de Janeiro (código 33).

```bash
curl -X 'GET' \
  'http://localhost:5119/api/v1/municipios/pegarTodosMunicipiosPorEstadoAsync/33' \
  -H 'accept: */*'
```

### 3. Dados de Doenças por Município

Consulta dados específicos de doenças (Dengue, Chikungunya, Zika) para um município pelo código IBGE.

```bash
curl -X 'POST' \
  'http://localhost:5119/api/v1/infodengue/pegarDadosPorCodigoIbge' \
  -H 'accept: */*' \
  -H 'Content-Type: application/json' \
  -d '{
  "cpf": "22822770077",
  "nome": "Weaur",
  "semanaInicio": 1,
  "semanaTermino": 50,
  "codigosIbge": 3304557
}'
```

### 4. Dados Sumarizados por Município

Obtém dados epidemiológicos sumarizados para um período específico.

```bash
curl -X 'POST' \
  'http://localhost:5119/api/v1/infodengue/getDadosSumarizados' \
  -H 'accept: */*' \
  -H 'Content-Type: application/json' \
  -d '{
  "cpf": "99219006090",
  "nome": "Xayflumo",
  "semanaInicio": 1,
  "semanaTermino": 51
}'
```

### 5. Dados Comparativos entre Rio de Janeiro e São Paulo

Compara dados epidemiológicos entre os municípios do Rio de Janeiro e São Paulo.

```bash
curl -X 'POST' \
  'http://localhost:5119/api/v1/infodengue/getDadosRjeSp' \
  -H 'accept: */*' \
  -H 'Content-Type: application/json' \
  -d '{
  "cpf": "99219006090",
  "nome": "Xayflumo",
  "semanaInicio": 1,
  "semanaTermino": 51
}'
```

### 6. Lista de Solicitantes

Retorna todos os usuários que fizeram solicitações à API.

```bash
curl -X 'GET' \
  'http://localhost:5119/api/v1/solicitantes' \
  -H 'accept: */*'
```

### 7. Histórico de Relatórios

Exibe um histórico de todos os relatórios pesquisados através da API.

```bash
curl -X 'GET' \
  'http://localhost:5119/api/v1/relatorios' \
  -H 'accept: */*'
```

## Parâmetros Comuns

- **cpf**: CPF válido do solicitante (utilize os CPFs fornecidos para teste)
- **nome**: Nome do solicitante correspondente ao CPF
- **semanaInicio**: Semana inicial do período de consulta (1-52)
- **semanaTermino**: Semana final do período de consulta (1-52)
- **codigosIbge**: Código IBGE do município (ex: 3304557 para Rio de Janeiro)

## Observações

- Todas as solicitações de dados que requerem autenticação devem incluir CPF e nome válidos
- O período de consulta é definido por semanas epidemiológicas (1-52)
- Para consultas de múltiplos municípios, utilize endpoints específicos como "getDadosRjeSp"
- O banco de dados utiliza SQL Server e requer configuração adequada na string de conexão
- Certifique-se de que as portas necessárias (5119 por padrão) estejam liberadas no firewall

## Requisitos do Sistema

- .NET Core SDK 8.0 ou superior
- SQL Server 2019 ou superior
- Acesso de administrador para restauração do banco de dados
- Aproximadamente 500MB de espaço em disco para o banco de dados

## Solução de Problemas

Se encontrar problemas ao iniciar a API:

1. Verifique se a string de conexão no `appsettings.json` está correta
2. Confirme que o banco de dados foi restaurado corretamente
3. Certifique-se de que todas as migrations foram aplicadas
4. Verifique os logs da aplicação para identificar erros específicos