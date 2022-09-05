-- Verify admin:tenants on sqlite

BEGIN;

SELECT id, fqdn, timestamp
from tenants;

ROLLBACK;
