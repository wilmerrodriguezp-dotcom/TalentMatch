CREATE USER 'talentmatch_app'@'localhost' IDENTIFIED BY 'TU_CLAVE_LOCAL';
GRANT SELECT, INSERT ON TalentMatchDb.Users TO 'talentmatch_app'@'localhost';