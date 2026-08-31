-- Script de inicialização dos bancos de dados
SELECT 'CREATE DATABASE users'
WHERE NOT EXISTS (SELECT FROM pg_database WHERE datname = 'users')\gexec

SELECT 'CREATE DATABASE catalog'
WHERE NOT EXISTS (SELECT FROM pg_database WHERE datname = 'catalog')\gexec
