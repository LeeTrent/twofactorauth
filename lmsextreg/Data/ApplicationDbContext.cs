using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using lmsextreg.Models;
using lmsextreg.Constants;

namespace lmsextreg.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Agency> Agencies { get; set; }
        public DbSet<SubAgency> SubAgencies { get; set; }
        public DbSet<LMSProgram> LMSPrograms { get; set; }
        public DbSet<ProgramApprover> ProgramApprovers { get; set; }
        public DbSet<EnrollmentStatus> EnrollmentStatuses { get; set; }
        public DbSet<ProgramEnrollment> ProgramEnrollments { get; set; }
        public DbSet<StatusTransition> StatusTransitions { get; set; }
        public DbSet<EnrollmentHistory> EnrollmentHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.HasDefaultSchema(MiscConstants.DB_SCHEMA_NAME);
          
            /***************************************************************************
             Customize the ASP.NET Identity model and override the defaults if needed.
             For example, you can rename the ASP.NET Identity table names and more.
             Add your customizations after calling base.OnModelCreating(builder);
             ***************************************************************************/
            builder.Entity<Agency>().ToTable("Agency");
            builder.Entity<SubAgency>().ToTable("SubAgency");
            builder.Entity<LMSProgram>().ToTable("LMSProgram");
            builder.Entity<ProgramApprover>().ToTable("ProgramApprover");
            builder.Entity<EnrollmentStatus>().ToTable("EnrollmentStatus");
            builder.Entity<ProgramEnrollment>().ToTable("ProgramEnrollment");
            builder.Entity<StatusTransition>().ToTable("StatusTransition");
            builder.Entity<EnrollmentHistory>().ToTable("EnrollmentHistory");

            // builder.Entity<Agency>().ToTable("Agency",                          schema: MiscConstants.DB_SCHEMA_NAME );
            // builder.Entity<SubAgency>().ToTable("SubAgency",                    schema: MiscConstants.DB_SCHEMA_NAME );
            // builder.Entity<LMSProgram>().ToTable("LMSProgram",                  schema: MiscConstants.DB_SCHEMA_NAME );
            // builder.Entity<ProgramApprover>().ToTable("ProgramApprover",        schema: MiscConstants.DB_SCHEMA_NAME );
            // builder.Entity<EnrollmentStatus>().ToTable("EnrollmentStatus",      schema: MiscConstants.DB_SCHEMA_NAME );
            // builder.Entity<ProgramEnrollment>().ToTable("ProgramEnrollment",    schema: MiscConstants.DB_SCHEMA_NAME );
            // builder.Entity<StatusTransition>().ToTable("StatusTransition",      schema: MiscConstants.DB_SCHEMA_NAME );
            // builder.Entity<EnrollmentHistory>().ToTable("EnrollmentHistory",    schema: MiscConstants.DB_SCHEMA_NAME );

            // builder.Entity<Agency>().ToTable("Agency",                          schema: "lmsext" );
            // builder.Entity<SubAgency>().ToTable("SubAgency",                    schema: "lmsext" );
            // builder.Entity<LMSProgram>().ToTable("LMSProgram",                  schema: "lmsext" );
            // builder.Entity<ProgramApprover>().ToTable("ProgramApprover",        schema: "lmsext" );
            // builder.Entity<EnrollmentStatus>().ToTable("EnrollmentStatus",      schema: "lmsext" );
            // builder.Entity<ProgramEnrollment>().ToTable("ProgramEnrollment",    schema: "lmsext" );
            // builder.Entity<StatusTransition>().ToTable("StatusTransition",      schema: "lmsext" );
            // builder.Entity<EnrollmentHistory>().ToTable("EnrollmentHistory",    schema: "lmsext" );

            /************************************************************************
             There are some configurations that can only be done with the fluent API
             (specifying a composite PK).
             ************************************************************************/            
            
            /////////////////////////////////////////////////////////////////////////
            //ProgramApprover: 
            // - Composite Primary Key
            /////////////////////////////////////////////////////////////////////////
            builder.Entity<ProgramApprover>()
                .HasKey( pa => new { pa.LMSProgramID, pa.ApproverUserId } );      

           /////////////////////////////////////////////////////////////////////////
            // ProgramApprover:
            //  - Foreign Key (LMSProgram.LMSProgramID)
            /////////////////////////////////////////////////////////////////////////
            // builder.Entity<ProgramApprover>()
            //     .HasOne( pa => pa.LMSProgram)
            //     .WithMany()
            //     .HasForeignKey(pa => pa.LMSProgramID);         

           /////////////////////////////////////////////////////////////////////////
            // ProgramApprover:
            //  - Foreign Key (ApplicationUser.Id)
            /////////////////////////////////////////////////////////////////////////
            builder.Entity<ProgramApprover>()
                .HasOne( pa => pa.Approver)
                .WithMany()
                .HasForeignKey(pa => pa.ApproverUserId);                                                     
            
            /////////////////////////////////////////////////////////////////////////                           
            // ProgramEnrollment:
            // - Unique Key Constraint Combination (LMSProgramID, LearnerUserId)
            /////////////////////////////////////////////////////////////////////////
            builder.Entity<ProgramEnrollment>()
                .HasIndex( p => new {p.LMSProgramID, p.StudentUserId} )
                .IsUnique();

            /////////////////////////////////////////////////////////////////////////
            // ProgramEnrollment:
            //  - Foreign Key (EnrollmentStatus.StatusCode)
            /////////////////////////////////////////////////////////////////////////
            builder.Entity<ProgramEnrollment>()
                .HasOne( pe => pe.EnrollmentStatus)
                .WithMany()
                .HasForeignKey(pe => pe.StatusCode);

           /////////////////////////////////////////////////////////////////////////
            // ProgramEnrollment:
            //  - Foreign Key (ApplicationUser.Id -  Student)
            /////////////////////////////////////////////////////////////////////////
            builder.Entity<ProgramEnrollment>()
                .HasOne( pe => pe.Student)
                .WithMany()
                .HasForeignKey(pe => pe.StudentUserId);

           /////////////////////////////////////////////////////////////////////////
            // ProgramEnrollment:
            //  - Foreign Key (ApplicationUser.Id - Approver)
            /////////////////////////////////////////////////////////////////////////
            builder.Entity<ProgramEnrollment>()
                .HasOne( pe => pe.Approver)
                .WithMany()
                .HasForeignKey(pe => pe.ApproverUserId);
                
            /////////////////////////////////////////////////////////////////////////                           
            // EnrollmentStatus:
            // - Unique Key Constraint
            /////////////////////////////////////////////////////////////////////////
            builder.Entity<EnrollmentStatus>()
                .HasIndex(es => es.StatusLabel)
                .IsUnique();                         
            
            /////////////////////////////////////////////////////////////////////////                           
            // StatusTransition:
            // - Unique Key Constraint Combination (FromStatusCode, ToStatusCode)
            /////////////////////////////////////////////////////////////////////////
            builder.Entity<StatusTransition>()
                .HasIndex( st => new {st.FromStatusCode, st.ToStatusCode} )
                .IsUnique();

            /////////////////////////////////////////////////////////////////////////
            // StatusTransition:
            //  - Foreign Key (EnrollmentStatus.StatusCode)
            /////////////////////////////////////////////////////////////////////////
            builder.Entity<StatusTransition>()
                .HasOne( st => st.FromStatus)
                .WithMany()
                .HasForeignKey(st => st.FromStatusCode);

           /////////////////////////////////////////////////////////////////////////
            // StatusTransition:
            //  - Foreign Key (EnrollmentStatus.StatusCode)
            /////////////////////////////////////////////////////////////////////////
            builder.Entity<StatusTransition>()
                .HasOne( st => st.ToStatus)
                .WithMany()
                .HasForeignKey(st => st.ToStatusCode);     

            /////////////////////////////////////////////////////////////////////////
            // EnrollmentHistory:
            //  - Foreign Key (ProgramEnrollment.ProgramEnrollmentID)
            /////////////////////////////////////////////////////////////////////////
            // builder.Entity<EnrollmentHistory>()
            //     .HasOne( eh => eh.ProgramEnrollment)
            //     .WithMany()
            //     .HasForeignKey(eh => eh.ProgramEnrollmentID);      

            /////////////////////////////////////////////////////////////////////////
            // EnrollmentHistory:
            //  - Foreign Key (StatusTransition.StatusTransitionID)
            /////////////////////////////////////////////////////////////////////////
            builder.Entity<EnrollmentHistory>()
                .HasOne( eh => eh.StatusTransition)
                .WithMany()
                .HasForeignKey(eh => eh.StatusTransitionID);   

            /////////////////////////////////////////////////////////////////////////
            // EnrollmentHistory:
            //  - Foreign Key (StatusTransition.StatusTransitionID)
            /////////////////////////////////////////////////////////////////////////
            builder.Entity<EnrollmentHistory>()
                .HasOne( eh => eh.Actor)
                .WithMany()
                .HasForeignKey(eh => eh.ActorUserId);
        }
    }
}