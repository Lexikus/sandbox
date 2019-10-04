import { Component } from '@angular/core';
import { PainterService } from '../services/painter.service';
import { MatDialog } from '@angular/material/dialog';
import { AppDialogComponent } from '../app-dialog/app-dialog.component';
import { SpriteSheetInterface } from '../interfaces/sprite-sheet.interface';

@Component({
	selector: 'app-sidebar',
	templateUrl: './app-sidebar.component.html',
	styleUrls: ['./app-sidebar.component.scss']
})
export class AppSidebarComponent {

	private _painter: PainterService;
	private _dialog: MatDialog;
	public brushes = [];

	constructor(painter: PainterService, dialog: MatDialog) {
		this._painter = painter;
		this._dialog = dialog;

		this._painter.SpriteSheet$.subscribe(e => {
			this.brushes.push(e);
		})
	}

	public OpenDialog() {
		const dialogRef = this._dialog.open(AppDialogComponent);
		dialogRef.afterClosed().subscribe(result => this.OnCloseDialog(result));
	}

	private OnCloseDialog(result: SpriteSheetInterface) {
		// dialog has been closed
		if (result === undefined) return;
		// the user didn't import an image.
		if (result.name === undefined) return;

		this._painter.AddImage(result.name, result);
	}
}
