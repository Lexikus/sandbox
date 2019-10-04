import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { PainterService } from '../services/painter.service';
import { SpriteSheetInterface } from '../interfaces/sprite-sheet.interface';

@Component({
	selector: 'app-drawer',
	templateUrl: './app-drawer.component.html',
	styleUrls: ['./app-drawer.component.scss']
})
export class AppDrawerComponent implements OnInit {

	private _painter: PainterService;

	@ViewChild("myCanvas") private _canvasRef: ElementRef<HTMLCanvasElement>;
	private _context: CanvasRenderingContext2D;

	private _mouseLeftPressed: boolean = false;
	private _mouseRightPressed: boolean = false;

	constructor(painter: PainterService) {
		this._painter = painter;
	}

	public ngOnInit() {
		this._context = this._canvasRef.nativeElement.getContext('2d');

		this._painter.TileSize$.subscribe(() => this.UpdateDrawerState());
		this._painter.Col$.subscribe(() => this.UpdateDrawerState());
		this._painter.Row$.subscribe(() => this.UpdateDrawerState());
		this._painter.DrawCell$.subscribe(e =>
			this.DrawCell(e.gridX, e.gridY, e.gridTileSize, e.spriteX, e.spriteY, e.spriteTileSize, e.sprite)
		)
		this._painter.ClearCell$.subscribe(e =>
			this.ClearCell(e.gridX, e.gridY, e.gridTileSize)
		)
	}

	private UpdateDrawerState() {
		this.ClearGrid();
		this.SetCanvasProperties(this._painter.Width, this._painter.Height)
		this.DrawGrid(this._painter.Row, this._painter.Col, this._painter.TileSize);
		this.DrawGridValues();
	}

	private SetCanvasProperties(width: number, height: number) {
		this._context.canvas.width = width + 1;
		this._context.canvas.height = height + 1;
	}

	private ClearGrid() {
		this._context.clearRect(0, 0, this._painter.Width, this._painter.Height);
	}

	private DrawGrid(row: number, col: number, tileSize: number): void {
		for (let x = 0; x <= col; x++) {
			for (let y = 0; y <= row; y++) {
				this.ClearCell(x, y, tileSize);
			}
		}
	}

	private DrawCellOnMousePosition(x: number, y: number, imageInformation: SpriteSheetInterface) {
		const selectedImageX = this._painter.SelectedX;
		const selectedImageY = this._painter.SelectedY;

		this._painter.AddToGrid(x, y, selectedImageX, selectedImageY, imageInformation.name);
	}

	private ClearCellOnMousePosition(x: number, y: number) {
		this._painter.RemoveFromGrid(x, y);
	}

	private DrawCell(gridX: number, gridY: number, gridTileSize: number,
		spriteX: number, spriteY: number, spriteTileSize: number,
		sprite: CanvasImageSource) {
		this._context.beginPath();
		this._context.translate(0.5, 0.5);
		this._context.drawImage(sprite,
			spriteX * spriteTileSize,
			spriteY * spriteTileSize,
			spriteTileSize,
			spriteTileSize,
			gridX * gridTileSize - 0.5,
			gridY * gridTileSize - 0.5,
			gridTileSize + 1,
			gridTileSize + 1
		);
		this._context.setTransform(1, 0, 0, 1, 0, 0);
		this._context.closePath();
	}

	private ClearCell(gridX: number, gridY: number, gridTileSize: number) {
		this._context.beginPath();
		this._context.translate(0.5, 0.5);
		this._context.fillStyle = "#fafafa";
		this._context.fillRect(gridX * gridTileSize, gridY * gridTileSize, gridTileSize, gridTileSize);

		this._context.rect(gridX * gridTileSize, gridY * gridTileSize, gridTileSize, gridTileSize);
		this._context.strokeStyle = "black";
		this._context.lineWidth = 1;
		this._context.stroke();
		this._context.setTransform(1, 0, 0, 1, 0, 0);
		this._context.closePath();
	}

	private DrawGridValues() {
		for (let x = 0; x < this._painter.Col; x++) {
			for (let y = 0; y < this._painter.Row; y++) {
				const gridInformation = this._painter.GetFromGrid(x, y);
				gridInformation.forEach(e => {
					const imageInformation = this._painter.GetImage(e.sprite);
					this.DrawCell(x, y, this._painter.TileSize, e.x, e.y, imageInformation.tileSize, imageInformation.image);
				});
			}
		}
	}

	public OnMouseMove(event): void {
		if (this._mouseLeftPressed) {
			const imageInformation = this._painter.GetActiveImage();

			if (imageInformation === undefined) return;

			const { offsetX, offsetY } = event;
			const x = Math.floor(offsetX / this._painter.TileSize);
			const y = Math.floor(offsetY / this._painter.TileSize);

			this.DrawCellOnMousePosition(x, y, imageInformation);
		} else if (this._mouseRightPressed) {
			const { offsetX, offsetY } = event;
			const x = Math.floor(offsetX / this._painter.TileSize);
			const y = Math.floor(offsetY / this._painter.TileSize);

			this.ClearCellOnMousePosition(x, y);
		}
	}

	public OnMouseDown(event) {
		if (event.button === 0) this._mouseLeftPressed = true;
		else if (event.button === 2) this._mouseRightPressed = true;

		this.OnMouseMove(event);
	}

	public OnMouseUp() {
		this._mouseLeftPressed = false;
		this._mouseRightPressed = false;
	}

	public OnContextMenu(event) {
		event.preventDefault();
	}
}
