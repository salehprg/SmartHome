import { Component } from "@angular/core";
import { interval } from 'rxjs';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { LayoutService } from "src/app/pages/_layout/layout.service";

@Component({
  selector: 'sh-settings-header',
  templateUrl: './settings-header.component.html'
})
export class SettingsHeaderComponent {
  now: Date;

  constructor(
      private layoutService : LayoutService
    ) {
  }

  getLogo() {
    const themeType = this.layoutService.getProp('theme.type') || 'light';
    if (themeType === 'light') return './assets/media/logos/logo-text.svg';
    else return './assets/media/logos/logo-text-white.svg';
  }

}
