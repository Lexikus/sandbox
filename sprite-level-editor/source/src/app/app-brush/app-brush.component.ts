import { Component, OnInit, Input, ViewChild, ElementRef } from '@angular/core';
import { SpriteSheetInterface } from '../interfaces/sprite-sheet.interface';
import { PainterService } from '../services/painter.service';
import { skip } from 'rxjs/operators';

@Component({
	selector: 'app-brush',
	templateUrl: './app-brush.component.html',
	styleUrls: ['./app-brush.component.scss']
})
export class AppBrushComponent implements OnInit, SpriteSheetInterface {
	@Input() public image: HTMLImageElement;
	@Input() public name: string;
	@Input() public type: string;
	@Input() public imageBuffer: ArrayBuffer;
	@Input() public tileSize: number;
	@Input() public width: number;
	@Input() public height: number;
	@ViewChild("brushCanvas") private _canvasRef: ElementRef<HTMLCanvasElement>;

	private _painter: PainterService;

	private _context: CanvasRenderingContext2D;

	constructor(painter: PainterService) {
		this._painter = painter;
		this._painter.SelectedImage$.subscribe((name) => {
			if (name !== this.name) {
				this.OnSelectedImageChanged();
			}
		});
	}

	public ngOnInit(): void {
		this._context = this._canvasRef.nativeElement.getContext('2d');

		// FIXME: We have to draw the canvas the first time asynchronously. This should work sync. it doesn't tho. Check!
		setTimeout(() => this.DrawGrid(this.width, this.height, this.tileSize));
	}

	private ClearCanvas() {
		this._context.clearRect(0, 0, this.width, this.height);
	}

	private DrawGrid(width: number, height: number, tileSize: number) {

		const rows = Math.floor(height / tileSize);
		const cols = Math.floor(width / tileSize);

		for (let x = 0; x <= cols; x++) {
			this._context.moveTo(x * tileSize, 0);
			this._context.lineTo(x * tileSize, rows * tileSize);
		}


		for (let y = 0; y <= rows; y++) {
			this._context.moveTo(0, y * tileSize);
			this._context.lineTo(cols * tileSize, y * tileSize);
		}

		this._context.strokeStyle = "grey";
		this._context.lineWidth = 1;
		this._context.stroke();
	}

	private SelectCell(x: number, y: number) {
		this._painter.SelectedX = x;
		this._painter.SelectedY = y;
		this._painter.SelectedImage = this.name;

		this.ClearCanvas();
		this.DrawGrid(this.width, this.height, this.tileSize);

		this._context.fillStyle = "rgba(0, 0, 255, 0.5)";
		this._context.fillRect(x * this.tileSize, y * this.tileSize, this.tileSize, this.tileSize);
	}

	public OnCanvasClick(event) {
		const { offsetX, offsetY } = event;
		const x = Math.floor(offsetX / this.tileSize);
		const y = Math.floor(offsetY / this.tileSize);

		this.SelectCell(x, y);
	}

	private OnSelectedImageChanged() {
		if (this._context === undefined) return;

		this.ClearCanvas();
		this.DrawGrid(this.width, this.height, this.tileSize);
	}
}
