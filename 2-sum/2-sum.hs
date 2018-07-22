{- Algorithms 2 - Graph Search, Shortest Paths, and Data Structures
   Stanford University via coursera.org

   Programming Assignment #4 - Variation of the 2-Sum algorithm

   Remarks:  This is a Haskell version of the C# program.

   Author: Jason Hooper
-}

import Data.List (nub)

type Sum = Integer

getSums :: [Integer] -> [Sum]
getSums numbers = nub $ go numbers (reverse numbers)
  where
    go :: [Integer] -> [Integer] -> [Sum]
    go [] _  = []
    go _  [] = []
    go (a:as) (b:bs) | a + b > 10000    = go (a:as) bs
                     | a + b < (-10000) = go as (b:bs)
                     | otherwise        = inner a (b:bs) ++ go as (b:bs)

    inner :: Integer -> [Integer] -> [Sum]
    inner a = takeWhile (>(-10000))
              . map (+a)
              . filter (/=a)

main :: IO ()
main = do
  file <- readFile "2sum-sorted.txt"
  let numbers = (map read . lines $ file) :: [Integer]
  let sums = getSums numbers
  print $ (sums, length sums)

