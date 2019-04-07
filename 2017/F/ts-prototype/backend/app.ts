import * as Coll from 'typescript-collections';
import * as I from '@app/interfaces';

type Cell  = '*' | '.' | '#';

type Coords = readonly [number, number];
type Matrix = readonly (readonly Cell[])[]; // optimize to jagged array

class MaxSightsCalculator {
    cache = new Coll.Dictionary<Coords, I.Nullable<number>>(
        ([i, j]: Coords) => `[${i} : ${j}]`
    );
    
    get rows() { return this.matrix.length; }
    get cols() { return this.matrix[0].length; }

    constructor(readonly matrix: Matrix) {}

    calculate(i = 0, j = 0): I.Nullable<number> {
        const { rows, cols, cache } = this;
        if (i >= rows || j >= cols) {
            return null;
        }        
        const cached = cache.getValue([i, j]);
        if (cached != null) {
            return cached;
        }
        const result = this.impl(i, j);
        this.cache.setValue([i, j], result);
        return result;
    } 

    cellToSights(cell: Cell) {
        return cell === '#' ? null : Number(cell === '*');
    }

    impl(i = 0, j = 0) {
        const { matrix, rows, cols } = this;
        const cell = this.cellToSights(matrix[i][j]);

        if (i === rows - 1 && j === cols - 1) {
            return cell;
        }
        if (cell == null) {
            return null;
        }
        const rightResult = this.calculate(i, j + 1);
        const downResult  = this.calculate(i + 1, j);
        if (rightResult == null && downResult == null) {
            return null;
        }

        return cell + (
            rightResult == null ? downResult  :
            downResult  == null ? rightResult :
            Math.max(rightResult, downResult)
        )!;
    }

}

const calc = new MaxSightsCalculator([
    ['*','*','.','*','.'],
    ['.','.','*','#','.'],
    ['*','#','*','.','.'],
    ['*','.','*','.','*']
]);
console.dir(calc.calculate());
console.dir(calc.cache);