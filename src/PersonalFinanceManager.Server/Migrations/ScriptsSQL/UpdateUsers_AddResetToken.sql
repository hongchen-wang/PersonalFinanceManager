ALTER TABLE "Users" ADD COLUMN IF NOT EXISTS "ResetToken" TEXT;
ALTER TABLE "Users" ADD COLUMN IF NOT EXISTS "ResetTokenExpiration" Timestamptz; -- timestamp with time zone