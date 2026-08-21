USE BackendAssessmentDb;
GO

-- ============================================================
-- 2. SEED DATA 
-- ============================================================
DECLARE @Token NVARCHAR(50) = 'VEH-EDI_BACKEND';

-- REQ-001: Normal Case
INSERT INTO Plannings (RequestCode, CandidateToken, CreatedAt, Status, Version) 
VALUES ('REQ-001', @Token, GETUTCDATE(), 'PROCESSED', 1);
INSERT INTO PlanningSlots (PlanningId, SlotOrder, SlotName, OriginalQuantity, BalancedQuantity, IsActive)
VALUES 
    (SCOPE_IDENTITY(), 0, 'Senin', 4, 4, 1),
    (SCOPE_IDENTITY(), 1, 'Selasa', 5, 5, 1),
    (SCOPE_IDENTITY(), 2, 'Rabu', 1, 4, 1),
    (SCOPE_IDENTITY(), 3, 'Kamis', 7, 5, 1),
    (SCOPE_IDENTITY(), 4, 'Jumat', 6, 5, 1),
    (SCOPE_IDENTITY(), 5, 'Sabtu', 4, 4, 1),
    (SCOPE_IDENTITY(), 6, 'Minggu', 0, 0, 0);

-- REQ-002: Total Habis Dibagi
INSERT INTO Plannings (RequestCode, CandidateToken, CreatedAt, Status, Version) 
VALUES ('REQ-002', @Token, GETUTCDATE(), 'PROCESSED', 1);
INSERT INTO PlanningSlots (PlanningId, SlotOrder, SlotName, OriginalQuantity, BalancedQuantity, IsActive)
VALUES 
    (SCOPE_IDENTITY(), 0, 'Senin', 2, 2, 1),
    (SCOPE_IDENTITY(), 1, 'Selasa', 2, 2, 1),
    (SCOPE_IDENTITY(), 2, 'Rabu', 2, 2, 1);

-- REQ-003: Semua 0
INSERT INTO Plannings (RequestCode, CandidateToken, CreatedAt, Status, Version) 
VALUES ('REQ-003', @Token, GETUTCDATE(), 'PROCESSED', 1);
INSERT INTO PlanningSlots (PlanningId, SlotOrder, SlotName, OriginalQuantity, BalancedQuantity, IsActive)
VALUES 
    (SCOPE_IDENTITY(), 0, 'Senin', 0, 0, 0),
    (SCOPE_IDENTITY(), 1, 'Selasa', 0, 0, 0),
    (SCOPE_IDENTITY(), 2, 'Rabu', 0, 0, 0);

-- REQ-004: Satu Slot Aktif
INSERT INTO Plannings (RequestCode, CandidateToken, CreatedAt, Status, Version) 
VALUES ('REQ-004', @Token, GETUTCDATE(), 'PROCESSED', 1);
INSERT INTO PlanningSlots (PlanningId, SlotOrder, SlotName, OriginalQuantity, BalancedQuantity, IsActive)
VALUES 
    (SCOPE_IDENTITY(), 0, 'Senin', 0, 0, 0),
    (SCOPE_IDENTITY(), 1, 'Selasa', 10, 10, 1),
    (SCOPE_IDENTITY(), 2, 'Rabu', 0, 0, 0);

-- REQ-005: Tie (rencana sama) + Total bersisa
INSERT INTO Plannings (RequestCode, CandidateToken, CreatedAt, Status, Version) 
VALUES ('REQ-005', @Token, GETUTCDATE(), 'PROCESSED', 1);
INSERT INTO PlanningSlots (PlanningId, SlotOrder, SlotName, OriginalQuantity, BalancedQuantity, IsActive)
VALUES 
    (SCOPE_IDENTITY(), 0, 'Senin', 5, 6, 1),
    (SCOPE_IDENTITY(), 1, 'Selasa', 5, 5, 1),
    (SCOPE_IDENTITY(), 2, 'Rabu', 5, 5, 1);

-- REQ-006: Nilai Besar
INSERT INTO Plannings (RequestCode, CandidateToken, CreatedAt, Status, Version) 
VALUES ('REQ-006', @Token, GETUTCDATE(), 'PROCESSED', 1);
INSERT INTO PlanningSlots (PlanningId, SlotOrder, SlotName, OriginalQuantity, BalancedQuantity, IsActive)
VALUES 
    (SCOPE_IDENTITY(), 0, 'Senin', 1000000, 1000000, 1),
    (SCOPE_IDENTITY(), 1, 'Selasa', 2000000, 2000000, 1),
    (SCOPE_IDENTITY(), 2, 'Rabu', 3000000, 3000000, 1);

-- REQ-007: Total Bersisa (Prioritas ke terbesar)
INSERT INTO Plannings (RequestCode, CandidateToken, CreatedAt, Status, Version) 
VALUES ('REQ-007', @Token, GETUTCDATE(), 'PROCESSED', 1);
INSERT INTO PlanningSlots (PlanningId, SlotOrder, SlotName, OriginalQuantity, BalancedQuantity, IsActive)
VALUES 
    (SCOPE_IDENTITY(), 0, 'Senin', 3, 4, 1),
    (SCOPE_IDENTITY(), 1, 'Selasa', 3, 3, 1),
    (SCOPE_IDENTITY(), 2, 'Rabu', 4, 4, 1);

-- REQ-008:
INSERT INTO Plannings (RequestCode, CandidateToken, CreatedAt, Status, Version) 
VALUES ('REQ-008', @Token, GETUTCDATE(), 'PROCESSED', 2);
INSERT INTO PlanningSlots (PlanningId, SlotOrder, SlotName, OriginalQuantity, BalancedQuantity, IsActive)
VALUES 
    (SCOPE_IDENTITY(), 0, 'Senin', 2, 2, 1),
    (SCOPE_IDENTITY(), 1, 'Selasa', 2, 2, 1);

-- REQ-009: Random Normal
INSERT INTO Plannings (RequestCode, CandidateToken, CreatedAt, Status, Version) 
VALUES ('REQ-009', @Token, GETUTCDATE(), 'PROCESSED', 1);
INSERT INTO PlanningSlots (PlanningId, SlotOrder, SlotName, OriginalQuantity, BalancedQuantity, IsActive)
VALUES 
    (SCOPE_IDENTITY(), 0, 'Senin', 4, 4, 1),
    (SCOPE_IDENTITY(), 1, 'Selasa', 4, 4, 1);

-- REQ-010: Random + 0
INSERT INTO Plannings (RequestCode, CandidateToken, CreatedAt, Status, Version) 
VALUES ('REQ-010', @Token, GETUTCDATE(), 'PROCESSED', 1);
INSERT INTO PlanningSlots (PlanningId, SlotOrder, SlotName, OriginalQuantity, BalancedQuantity, IsActive)
VALUES 
    (SCOPE_IDENTITY(), 0, 'Senin', 0, 0, 0),
    (SCOPE_IDENTITY(), 1, 'Selasa', 3, 3, 1),
    (SCOPE_IDENTITY(), 2, 'Rabu', 3, 3, 1);

PRINT 'Seed data berhasil ditambahkan!';
GO

-- ============================================================
-- 3. TOTAL VALIDATION QUERY
-- ============================================================
SELECT 
    p.Id AS PlanningId,
    SUM(ps.OriginalQuantity) AS OriginalTotal,
    SUM(ps.BalancedQuantity) AS BalancedTotal,
    CASE 
        WHEN SUM(ps.OriginalQuantity) = SUM(ps.BalancedQuantity) THEN 'YES' 
        ELSE 'NO' 
    END AS IsTotalValid
FROM Plannings p
JOIN PlanningSlots ps ON p.Id = ps.PlanningId
GROUP BY p.Id
ORDER BY p.Id;
GO

-- ============================================================
-- 4. HISTORY QUERY
-- ============================================================
SELECT 
    p.RequestCode,
    p.CreatedAt,
    COUNT(CASE WHEN ps.IsActive = 1 THEN 1 END) AS JumlahSlotAktif,
    SUM(ps.OriginalQuantity) AS TotalAwal,
    SUM(ps.BalancedQuantity) AS TotalHasil,
    p.Status
FROM Plannings p
JOIN PlanningSlots ps ON p.Id = ps.PlanningId
GROUP BY p.RequestCode, p.CreatedAt, p.Status
ORDER BY p.CreatedAt DESC;
GO

-- ============================================================
-- 5. ANOMALY QUERY
-- ============================================================
-- 5a. Slot nonaktif dengan BalancedQuantity > 0
SELECT 'Nonaktif_Balanced>0' AS AnomalyType, PlanningId, SlotOrder
FROM PlanningSlots
WHERE IsActive = 0 AND BalancedQuantity > 0;
GO

-- 5b. Total tidak sama
SELECT p.Id AS PlanningId, 'TotalMismatch' AS AnomalyType
FROM Plannings p
JOIN PlanningSlots ps ON p.Id = ps.PlanningId
GROUP BY p.Id
HAVING SUM(ps.OriginalQuantity) != SUM(ps.BalancedQuantity);
GO

-- 5c. RequestCode ganda (seharusnya tidak ada karena Unique, tapi tetap dicek)
SELECT RequestCode, COUNT(*) AS DuplicateCount
FROM Plannings
GROUP BY RequestCode
HAVING COUNT(*) > 1;
GO

-- ============================================================
-- 6. LARGEST ADJUSTMENTS
-- ============================================================
SELECT TOP 3
    PlanningId,
    SlotOrder,
    ABS(OriginalQuantity - BalancedQuantity) AS PerubahanAbsolut
FROM PlanningSlots
ORDER BY PerubahanAbsolut DESC, SlotOrder ASC;
GO

-- ============================================================
-- 7. ATOMIC SAVE (Pseudocode/Transaction Script)
-- ============================================================
/*
BEGIN TRANSACTION;
INSERT INTO Plannings (...) VALUES (...);
INSERT INTO PlanningSlots (...), (...), ...;
COMMIT;
-- Jika salah satu insert gagal, otomatis ROLLBACK
*/

-- ============================================================
-- 8. LATEST PROCESSING VERSION
--    Tampilkan hanya run terbaru untuk setiap Planning.
-- ============================================================
WITH RankedPlannings AS (
    SELECT 
        p.*,
        ROW_NUMBER() OVER (PARTITION BY p.RequestCode ORDER BY p.Version DESC) AS rn
    FROM Plannings p
)
SELECT 
    Id,
    RequestCode,
    CandidateToken,
    CreatedAt,
    Status,
    Version,
    rn AS IsLatest
FROM RankedPlannings 
WHERE rn = 1
ORDER BY Id;
GO

-- ============================================================
-- 9. INDEX PROPOSAL
-- ============================================================
/*
Manfaat Index:
- IX_Plannings_RequestCode (UNIQUE): Mempercepat pencarian berdasarkan RequestCode.
- IX_Plannings_CreatedAt: Mempercepat sorting dan filter untuk history (ORDER BY CreatedAt DESC).
- IX_Plannings_Status: Mempercepat filter berdasarkan status.

Biaya Write:
- Setiap INSERT/UPDATE/DELETE harus update index, sehingga performa write sedikit lebih lambat.
- Memakan storage tambahan.
*/
-- CREATE UNIQUE INDEX IX_Plannings_RequestCode ON Plannings(RequestCode);
-- CREATE INDEX IX_Plannings_CreatedAt ON Plannings(CreatedAt);
-- CREATE INDEX IX_Plannings_Status ON Plannings(Status);
GO

-- ============================================================
-- 10. SAFE MIGRATION
--     Migrasi dari model lama (Slot1, Slot2, ... per kolom) 
--     menjadi model detail-per-row (PlanningSlots).
-- ============================================================
/*
PRE-MIGRATION (Validasi):
1. Backup tabel lama: SELECT * INTO Plannings_Backup FROM Plannings;
2. Validasi total data: SUM(Slot1 + Slot2 + ...) harus sama dengan SUM(OriginalQuantity) di tabel baru.

MIGRATION SCRIPT:
1. Buat tabel PlanningSlots dengan struktur detail-per-row.
2. INSERT INTO PlanningSlots (PlanningId, SlotOrder, SlotName, OriginalQuantity, BalancedQuantity)
   SELECT Id, 1, 'Senin', Slot1, Slot1 FROM OldPlannings;
   ... (ulangi untuk semua slot)

POST-MIGRATION (Validasi):
1. Bandingkan total agregat lama vs baru.
2. Cek tidak ada data hilang (COUNT).
3. Cek constraint (non-negatif, foreign key).
*/
GO