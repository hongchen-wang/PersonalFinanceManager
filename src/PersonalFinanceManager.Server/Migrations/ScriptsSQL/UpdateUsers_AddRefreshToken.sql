ALTER TABLE "Users" ADD COLUMN IF NOT EXISTS "RefreshToken" TEXT;
ALTER TABLE "Users" ADD COLUMN IF NOT EXISTS "RefreshTokenExpiration" Timestamp;