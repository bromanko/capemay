-- Verify admin:tenants on sqlite

BEGIN;

SELECT fqdn from tenants;

ROLLBACK;
