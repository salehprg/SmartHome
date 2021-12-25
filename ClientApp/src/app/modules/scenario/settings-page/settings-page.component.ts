import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { ScenarioSettingsService } from './../scenario-settings.service';
import { Component, OnInit, ViewEncapsulation, OnDestroy } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { HomeService } from '../../home/_services/home.service';
import Keyboard from 'simple-keyboard';
import layout from 'simple-keyboard-layouts/build/layouts/farsi';

@Component({
  selector: 'sh-scenario-settings-page',
  templateUrl: './settings-page.component.html',
  styleUrls: [
    './settings-page.component.scss',
    '../../../../../node_modules/simple-keyboard/build/css/index.css',
  ],
  encapsulation: ViewEncapsulation.None,
})
export class ScenarioSettingsPage implements OnDestroy {
  active: string;
  keyboard: Keyboard;
  nameValue: string;
  lang:string = "persian"

  lookups = [
    this.translate.instant('GENERAL.NAME'),
    this.translate.instant('GENERAL.STATE'),
  ];

  constructor(
    public homeService: HomeService,
    public translate: TranslateService,
    public scenarioSettings: ScenarioSettingsService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private home: HomeService
  ) {
    this.active = this.translate.instant('GENERAL.NAME');
    const id = activatedRoute.snapshot.params['id']
    if (id && id !== 'add') {
      const sc = home._scenarios$.value.find(s => s.scenarioId === +id);
      scenarioSettings.setEditMode(sc);
      this.scenarioSettings.setScenarioId(id)

      this.nameValue = sc.scenarioName

    }
    else{
      this.scenarioSettings.editMode = false
    }

  }

  ngAfterViewInit() {
    this.keyboard = new Keyboard({
      // debug: true,
      onChange: (input) => this.onChange(input),
      onKeyPress: (button) => this.onKeyPress(button),
      ...layout,
      rtl: true,
      theme: 'hg-theme-default myTheme1',
      preventMouseDownDefault: true, // If you want to keep focus on input
    });

    if(this.scenarioSettings.editMode){
      this.keyboard.setInput(this.scenarioSettings.scenario.scenarioName)
    }
  }

  onChange = (input: string) => {
    this.nameValue = input;
    this.scenarioSettings.setScenarioName(input)
  };

  onKeyPress = (button: string) => {
    if (button === '{shift}' || button === '{lock}') this.handleShift();
  };

  onInputChange = (event: any) => {
    this.keyboard.setInput(event.target.value);
    this.scenarioSettings.setScenarioName(event.target.value)
  };

  handleShift = () => {
    let currentLayout = this.keyboard.options.layoutName;
    let shiftToggle = currentLayout === 'default' ? 'shift' : 'default';

    this.keyboard.setOptions({
      layoutName: shiftToggle,
    });
  };

  changeTab(look) {
    this.active = look;

    if (look === 'State') {
      document.getElementById('keyboard').style.display = 'none';
    } else {
      document.getElementById('keyboard').style.display = 'block';

      if(this.lang === 'persian'){
        this.keyboard.setOptions({
          ...layout,
          rtl: true
        })
      }
      else{
        this.keyboard.setOptions({
          layout: {
            default: [
              "` 1 2 3 4 5 6 7 8 9 0 - = {bksp}",
              "{tab} q w e r t y u i o p [ ] \\",
              "{lock} a s d f g h j k l ; ' {enter}",
              "{shift} z x c v b n m , . / {shift}",
              ".com @ {space}",
            ],
            shift: [
              "~ ! @ # $ % ^ & * ( ) _ + {bksp}",
              "{tab} Q W E R T Y U I O P { } |",
              '{lock} A S D F G H J K L : " {enter}',
              "{shift} Z X C V B N M < > ? {shift}",
              ".com @ {space}",
            ],
          },
          rtl: false
        })
      }
    }
  }

  changeLang(event) {
    this.lang = event.target.value;

    if (this.lang === 'english') {
      this.keyboard.setOptions({
        layout: {
          default: [
            "` 1 2 3 4 5 6 7 8 9 0 - = {bksp}",
            "{tab} q w e r t y u i o p [ ] \\",
            "{lock} a s d f g h j k l ; ' {enter}",
            "{shift} z x c v b n m , . / {shift}",
            ".com @ {space}",
          ],
          shift: [
            "~ ! @ # $ % ^ & * ( ) _ + {bksp}",
            "{tab} Q W E R T Y U I O P { } |",
            '{lock} A S D F G H J K L : " {enter}',
            "{shift} Z X C V B N M < > ? {shift}",
            ".com @ {space}",
          ],
        },
        rtl: false
      })

    }
    else if (this.lang === 'persian') {
      this.keyboard.setOptions({
        ...layout,
        rtl: true
      })

    }

  }

  ngOnDestroy(): void {
    this.scenarioSettings.reset();
  }
}
