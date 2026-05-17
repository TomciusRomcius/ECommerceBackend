import { Component } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { RouterLink, RouterLinkActive } from '@angular/router';

@Component({
  selector: 'app-page-padder',
  imports: [MatIconModule, RouterLink, RouterLinkActive],
  templateUrl: './page-padder.html',
})
export class PagePadder {}
