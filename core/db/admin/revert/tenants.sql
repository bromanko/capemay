-- Revert admin:tenants from sqlite

BEGIN;

DROP TABLE IF EXISTS tenants;

COMMIT;
