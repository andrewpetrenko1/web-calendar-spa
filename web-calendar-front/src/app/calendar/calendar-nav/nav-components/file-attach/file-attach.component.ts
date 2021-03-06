import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FileSystemFileEntry, NgxFileDropEntry } from 'ngx-file-drop';
import { saveAs } from 'file-saver';
import { FileAttachService } from 'src/app/services/file-attach.service';
import { ToastGlobalService } from 'src/app/services/toast-global.service';
import { throwError } from 'rxjs';
import { CalendarEvent } from 'src/app/interfaces/event.interface';

@Component({
  selector: 'app-file-attach',
  templateUrl: './file-attach.component.html',
  styleUrls: ['./file-attach.component.css']
})
export class FileAttachComponent implements OnInit {
  @Input() event: CalendarEvent;
  @Input() public attachedFile: File;
  @Output() attachedFileChange = new EventEmitter<File>();

  public downloadFile: File = null;

  private readonly maxFileSize = 10485760;

  constructor(
    private toastService: ToastGlobalService,
    private fileService: FileAttachService
  ) { }

  ngOnInit(): void {
    this.attachedFileInit();
  }

  public dropped(newFiles: NgxFileDropEntry[]) {
    for (const droppedFile of newFiles) {
      if (droppedFile.fileEntry.isFile) {
        const fileEntry = droppedFile.fileEntry as FileSystemFileEntry;
        fileEntry.file((file: File) => {
          if(file.size > this.maxFileSize)
          {
            this.toastService.add({
              delay: 5000,
              title: 'Warning!',
              content: `File ${file.name} is too long`,
              className: "bg-warning text-light"
            });
            return;
          }
          this.attachedFile = file;
          this.attachedFileChange.emit(this.attachedFile);
        });
        break;
      }
    }
  }

  attachedFileInit() {
    if(this.event.id === null || this.event.id === undefined || this.event.fileId === null)
      return;
    
    this.fileService.getEventFile(this.event.id).subscribe(response => {
      this.downloadFile = new File([response.body], this.getFileName(response.headers), {type: response.body.type});
    }, err => {
      if(err.status === 404)
        this.downloadFile = null;
      if(err.status === 415)
        this.toastService.add({
          delay: 5000,
          title: 'Warning!',
          content: `File is too long`,
          className: "bg-warning text-light"
        });
      else
        throwError(err);
    });
  }

  getFileName(headers) {
    return decodeURI(headers.get('content-disposition').match(/filename\*=UTF-8''(.*?)$/)[1]);
  }

  download() {
    saveAs(this.downloadFile);
  }
  
}
