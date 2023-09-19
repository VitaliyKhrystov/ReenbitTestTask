import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  myForm : FormGroup;
    constructor(){
        this.myForm = new FormGroup({
          "userEmail": new FormControl("", [
                                Validators.required,
                                Validators.email
                            ]),
          "userFile": new FormControl(null, [
                                Validators.required,
                                this.requiredFileType])
        });
    }

  fileType: string = environment.fileType == null ? '':environment.fileType;

    submit(event:FormGroup){
        console.log(this.myForm);
    }

  requiredFileType (control: FormControl): {[s:string]:boolean}|null {
    const file = control.value;
    let pattern = "[^\s]+(.*?).(" + environment.fileType + ")$";
    const regex = new RegExp(pattern);
    if ( file ) {
      if ( !regex.test(file)) {
        return {"userFile": true};
      }
    };
    return null;

  }
}

