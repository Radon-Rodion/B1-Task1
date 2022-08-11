SELECT SUM(IntegerNumber) FROM Lines;

SELECT TOP (1) Percentile_Disc (0.5)
           WITHIN GROUP (ORDER BY DoubleNumber)
           OVER()
FROM Lines;