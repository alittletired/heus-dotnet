windows
docker run -d --name postgres -p 5432:5432 -e POSTGRES_USER=root -e POSTGRES_PASSWORD=mysecretpassword -e PGDATA=/var/lib/postgresql/data/pgdata -v d:/data/postgresql:/var/lib/postgresql/data postgres

docker run -d --name postgres -p 5432:5432 -e POSTGRES_USER=root -e POSTGRES_PASSWORD=6ec7b7d162b2 -e TZ=Asia/Shanghai -e PGDATA=/var/lib/postgresql/data/pgdata -v /Users/zhulin/dockerdata/postgresql:/var/lib/postgresql/data postgres

docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=YourStrongPassw0rd" \
 -p 1433:1433 --name sqlserver --hostname sqlserver -v /Users/zhulin/dockerdata/mssql/:/var/opt/mssql/ \
 -d \
 mcr.microsoft.com/mssql/server:2022-latest
