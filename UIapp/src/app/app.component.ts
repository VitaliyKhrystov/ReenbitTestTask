import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { FileService } from 'src/Services/file.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {

  myForm: FormGroup;
  fileType: string = environment.fileType == null ? '' : environment.fileType;
  file?: File;
  status: string = "";

    constructor(private fileService: FileService){
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


  onChange(event:any) {
    const file: File = event.target.files[0];
    if (file) {
      this.file = file;
    }
  }

  send(){
    if (this.file && this.myForm.value.userEmail) {
      const formData = new FormData();
      formData.append('email', this.myForm.value.userEmail);
      formData.append('file', this.file, this.file.name);

      this.fileService.upload(formData).subscribe({
        next: (status) => {
          console.log(`Status: ${status}. File is successfully uploaded!`);
          this.status = "File is successfully uploaded!"
        },
        error: err => {
          console.log(err);
          this.status = "File isn't uploaded!"
        }
      });
      this.myForm = new FormGroup({
          "userEmail": new FormControl(),
          "userFile": new FormControl()
      });
      setTimeout(() => {
        this.status = ""
      }, 5000);

    }
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

