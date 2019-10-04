//TODO: Directive for only numbers

import { Component, OnInit } from '@angular/core';
import { PainterService } from '../services/painter.service';

@Component({
	selector: 'app-settings',
	templateUrl: './app-settings.component.html',
	styleUrls: ['./app-settings.component.scss']
})
export class AppSettingsComponent implements OnInit {

	public columns: number;
	public rows: number;
	public tileSize: number;
	public max: number;

	private _painter: PainterService;

	constructor(painter: PainterService) {
		this._painter = painter;

		this._painter.TileSize$.subscribe((value) => this.tileSize = value);
		this._painter.Col$.subscribe((value) => this.columns = value);
		this._painter.Row$.subscribe((value) => this.rows = value);
	}

	public ngOnInit() {
		this.columns = this._painter.Col;
		this.rows = this._painter.Row;
		this.tileSize = this._painter.TileSize;
		this.max = this._painter.CAP;
	}

	public OnTileSizeChange() {
		this._painter.TileSize = this.tileSize;
	}

	public OnRowsChange() {
		this._painter.Row = this.rows;
	}

	public OnColumnsChange() {
		this._painter.Col = this.columns;
	}

}
