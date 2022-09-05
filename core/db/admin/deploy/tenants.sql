-- Deploy admin:tenants to sqlite

BEGIN;

CREATE TABLE tenants(
                        id        TEXT PRIMARY KEY,
                        fqdn      TEXT     NOT NULL UNIQUE,
                        timestamp DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
);

COMMIT;
