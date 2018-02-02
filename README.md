# PLCalc
Calculadora de distribuição de Participação nos Lucros

Documentação Swagger: http://localhost:53922/swagger/help/swagger.json

*********************************************************************

#Essa API implementa os seguintes recursos:

- GET /plcalc/funcionarios

Retorna lista de todos os funcionarios | Corpo da solicitação: Nenhum

- GET /plcalc/funcionarios/{matricula}

Retorna um unico funcionário selecionado pela matricula | Parametro: Matricula do funcionario | Corpo da solicitação: Nenhum

- POST /plcalc/funcionarios

Cadastra novo funcionario | Corpo da solicitação: Dados do funcionario

- POST /plcalc/funcionarios/Lista

Cadastra lista de novos funcionarios| Corpo da solicitação: Lista de Funcionarios

- PUT /plcalc/funcionarios/{matricula}

Atualiza dados de um funcionario cadastrado| Parametro: Matricula do funcionario | Corpo da solicitação: Dados do funcionário

- DELETE /plcalc/funcionarios/{matricula}

Deleta funcionario selecionado pela matricula | Parametro: Matricula do funcionario | Corpo da solicitação: Dados do funcionário


- GET /plcalc/funcionarios/participacoes/{saldo}

Retorna lista de participaçoes nos lucros para todos os funcionarios | Parametro: Saldo a distribuir | Corpo da solicitação: Nenhum

- GET /plcalc/funcionarios{matricula}/participacoes/{saldo}

Retorna participaçoes nos lucros para um unico funcionario selecionado pela matricula | Parametro: MAtricula do funcionario |Parametro:
Saldo a distribuir | Corpo da solicitação: Nenhum

