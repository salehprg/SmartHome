import { HttpClient } from "@angular/common/http";
import { Component, ViewEncapsulation } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { TranslateService } from "@ngx-translate/core";
import { NotifierService } from "angular-notifier";
import { Subscription } from "rxjs";
import Keyboard from 'simple-keyboard'
import layout from 'simple-keyboard-layouts/build/layouts/farsi'
import { WifiService } from "../header/wifi/_services/wifi.service";

@Component({
  selector: 'sh-wifi-password',
  templateUrl: './wifi-password.component.html',
  encapsulation: ViewEncapsulation.None,
  styleUrls: ['../../../../../node_modules/simple-keyboard/build/css/index.css', './wifi-password.component.scss']
})

export class WifiPasswordComponent {
  value = "";
  keyboard: Keyboard;
  lang: string = "english"
  // id: string

  constructor(
    private router: Router,
    private active: ActivatedRoute,
    private notifier: NotifierService,
    private wifiService: WifiService,
    private translate : TranslateService
  ) {
    // this.id = this.active.snapshot.params['id']
  }

  ngAfterViewInit() {
    this.keyboard = new Keyboard({
      onChange: input => this.onChange(input),
      onKeyPress: button => this.onKeyPress(button),
      // ...layout,
      // rtl: true,
      theme: 'hg-theme-default myTheme1',
    });
  }

  onChange = (input: string) => {
    this.value = input;
    this.keyboard.setInput(input)



  };

  onKeyPress = (button: string) => {
    if (button === "{shift}" || button === "{lock}") this.handleShift();
  };

  onInputChange = (event: any) => {
    this.keyboard.setInput(event.target.value);
    this.value = event.target.value



  };

  handleShift = () => {
    let currentLayout = this.keyboard.options.layoutName;
    let shiftToggle = currentLayout === "default" ? "shift" : "default";

    this.keyboard.setOptions({
      layoutName: shiftToggle
    });
  };

  setWifi() {

    // if (this.wifiService.verify(this.id, this.value) ) {
    //   this.notifier.notify('success', 'با موفقیت وصل شدید')
    //   setTimeout(() => this.router.navigateByUrl(''), 1000)
    // }
    // else {
    //   this.notifier.notify('error', 'رمز عبور نادرست است')
    // }

    this.wifiService.verify(this.value).subscribe(
      (res:any) => {

        if(res === true) {
          this.notifier.notify('success' , this.translate.instant('NOTIFICATION.CONNECTED_SUCCESSFULLY'))
          setTimeout(()=> this.router.navigateByUrl('') , 2000)
        }
        else this.notifier.notify('error' , this.translate.instant('NOTIFICATION.CONNECT_ERROR'))

      },
      err => {

        this.notifier.notify('error' , this.translate.instant('NOTIFICATION.CONNECT_ERROR'))
      }
    )
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
}
