import { ScenarioSettingsService } from 'src/app/modules/scenario/scenario-settings.service';
import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'sh-card',
  templateUrl: './card.component.html'
})
export class CardComponent {
  @Input() title: string;
  @Input() class: string;
  @Input() icon: string;
  @Input() iconFill: string;
  @Input() heightFull: boolean;
  @Input() headerClass: boolean;
  @Input() isScenarioSetting : boolean;
  @Input() id : string;
  @Output() onChange = new EventEmitter<any>();

  constructor(public scenarioSettings: ScenarioSettingsService) {}

  setActive(value) {
    this.onChange.emit(value);
  }

  getIcon() {

    return {
      'fa': true,
      'sh-text-color' : true,
      'sh-card-icon' : this.icon === 'plug',
      [`fa-${this.icon}`]: true
    }
  }
}
