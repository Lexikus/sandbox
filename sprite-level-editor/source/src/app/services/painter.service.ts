import { Injectable } from '@angular/core';
import { BehaviorSubject, Subject } from 'rxjs';
import { SpriteSheetInterface } from '../interfaces/sprite-sheet.interface';
import { Grid } from './grid';
import { GridEmptyCellInterface } from '../interfaces/grid-empty-cell.interface';
import { GridCellInterface } from '../interfaces/grid-cell.interface';

@Injectable({
	providedIn: 'root'
})
export class PainterService {

	public readonly CAP: number = 100;

	private _tileSizeSource = new BehaviorSubject<number>(32);
	private _rowSource = new BehaviorSubject<number>(10);
	private _colSource = new BehaviorSubject<number>(10);

	private _spriteSheetSource = new Subject<SpriteSheetInterface>();
	private _drawCellSource = new Subject<GridCellInterface>();
	private _clearCellSource = new Subject<GridEmptyCellInterface>();

	private _grid = new Grid(this.CAP);
	private _images = new Map<string, SpriteSheetInterface>();

	private _selectedImageSource = new BehaviorSubject<string>("");
	private _selectedX: number;
	private _selectedY: number;

	public get TileSize() { return this._tileSizeSource.value; }
	public get TileSize$() { return this._tileSizeSource; }
	public set TileSize(value) { this._tileSizeSource.next(value); }

	public get Row() { return this._rowSource.value; }
	public get Row$() { return this._rowSource; }
	public set Row(value) { this._rowSource.next(value); }

	public get Col() { return this._colSource.value }
	public get Col$() { return this._colSource; }
	public set Col(value) { this._colSource.next(value); }

	public get SpriteSheet$() { return this._spriteSheetSource; }
	public get DrawCell$() { return this._drawCellSource; }
	public get ClearCell$() { return this._clearCellSource; }

	public get Width() { return this._colSource.value * this._tileSizeSource.value; }
	public get Height() { return this._rowSource.value * this._tileSizeSource.value; }

	public get SelectedX() { return this._selectedX; }
	public set SelectedX(value) { this._selectedX = value; }

	public get SelectedY() { return this._selectedY; }
	public set SelectedY(value) { this._selectedY = value; }

	public get SelectedImage() { return this._selectedImageSource.value; }
	public get SelectedImage$() { return this._selectedImageSource; }
	public set SelectedImage(value) { this._selectedImageSource.next(value); }

	public AddImage(name: string, spriteSheet: SpriteSheetInterface) {
		this._images.set(name, spriteSheet);
		this._spriteSheetSource.next(spriteSheet);
	}

	public GetActiveImage(): SpriteSheetInterface {
		return this._images.get(this.SelectedImage);
	}

	public GetImage(name: string): SpriteSheetInterface {
		return this._images.get(name);
	}

	public GetImageInformations() {
		let info = [];
		this._images.forEach(e => {
			info.push({ name: e.name, image: e.imageBuffer, width: e.width, height: e.height, tileSize: e.tileSize, type: e.type });
		});
		return info;
	}

	public GetGridInformation() {
		let info = [];
		for (let x = 0; x < this.Col; x++) {
			for (let y = 0; y < this.Row; y++) {
				info.push({ x: x, y: y, grid: this.GetFromGrid(x, y) });
			}
		}
		return info;
	}

	public GetGridSettings() {
		return {
			tileSize: this.TileSize,
			columns: this.Col,
			rows: this.Row
		}
	}

	public SetImageInformations(info: [{ name: string, image: ArrayBuffer, width: number, height: number, tileSize: number, type: string }]): Promise<{}> {
		if (info === undefined) return;

		const promises = info.map(e => {
			return new Promise(resolve => {
				const img = new Image();

				img.onload = () => {
					this.AddImage(e.name, {
						name: e.name,
						tileSize: e.tileSize,
						imageBuffer: e.image,
						image: img,
						type: e.type,
						width: e.width,
						height: e.height
					});
					resolve(true);
				};

				img.src = `data:${e.image}`;
			});
		})

		return Promise.all(promises);
	}

	public SetGridInformation(info: [{ x: number, y: number, grid: [{ x: number, y: number, sprite: string }] }]) {
		if (info === undefined) return;
		info.forEach(e => {
			e.grid.forEach(g => {
				this.AddToGrid(e.x, e.y, g.x, g.y, g.sprite);
			});
		});
	}

	public SetGridSettings(info: { tileSize: number, columns: number, rows: number }) {
		if (info === undefined) return;
		this.TileSize = info.tileSize;
		this.Col = info.columns;
		this.Row = info.rows;
	}

	public AddToGrid(gridX: number, gridY: number, spriteX: number, spriteY: number, sprite: string) {
		if (!this._grid.Contains(gridX, gridY, spriteX, spriteY, sprite)) {
			this._grid.Add(gridX, gridY, spriteX, spriteY, sprite);
			this._drawCellSource.next({
				gridX: gridX,
				gridY: gridY,
				gridTileSize: this.TileSize,
				spriteX: spriteX,
				spriteY: spriteY,
				spriteTileSize: this._images.get(sprite).tileSize,
				sprite: this._images.get(sprite).image
			});
		}
	}

	public RemoveFromGrid(gridX: number, gridY: number) {
		this._grid.Remove(gridX, gridY);
		this._clearCellSource.next({ gridX: gridX, gridY: gridY, gridTileSize: this.TileSize });
	}

	public GetFromGrid(gridX: number, gridY: number): [{ x: number, y: number, sprite: string }] {
		return this._grid.Get(gridX, gridY)
	}
}
