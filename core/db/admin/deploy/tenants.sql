-- Deploy admin:tenants to sqlite

BEGIN;

CREATE TABLE tenants (
       fqdn TEXT PRIMARY KEY,
       timestamp DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
);

COMMIT;
