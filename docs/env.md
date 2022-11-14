docker run -d --name postgres -e POSTGRES_PASSWORD=mysecretpassword -e PGDATA=/var/lib/postgresql/data/pgdata -v d:/data/postgresql:/var/lib/postgresql/data postgres
