These certificates are for development purposes (e.g. running API tests locally).

How to create dev certificates:
openssl genrsa -aes256 -out $CANAME.key 4096
openssl req -x509 -new -nodes -key $CANAME.key -sha256 -days 1826 -out $CANAME.crt

Generate pfx from key and crt ???
openssl pkcs12 -export -out domain.name.pfx -inkey domain.name.key -in domain.name.crt

Set Env Variables:
    .WithEnvironment("Kestrel__Certificates__Default__Path", "/usr/local/share/ca-certificates/apitests_CA.pfx")
    .WithEnvironment("Kestrel__Certificates__Default__Password", "Pa55w0rd!")

In Dockerfile add
ADD YOUR_CERTIFICATE /usr/local/share/ca-certificates/
RUN update-ca-certificates

Make sure your certificates are added to the .gitignore