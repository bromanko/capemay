-- Revert admin:tenants from sqlite

BEGIN;

DROP tenants;

COMMIT;
